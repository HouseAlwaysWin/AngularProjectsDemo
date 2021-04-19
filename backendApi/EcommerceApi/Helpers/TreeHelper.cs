using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EcommerceApi.Helpers
{
    public class TreeNode<T>{
        public T Item {get;set;}
        public IEnumerable<TreeNode<T>> Children {get;set;}
    }

    public interface ITreeNode<T>
    {
        IEnumerable<T> Children { get; set; }
    }

    public static class TreeHelper
    {
        public static IEnumerable<TreeNode<TEntity>> GenerateTree<TEntity,RootId>(
            this IEnumerable<TEntity> collection,
            Func<TEntity,RootId> id,
            Func<TEntity,RootId> parentId,
            RootId rootId=default(RootId)
        ){
            foreach(var item in collection.Where(c => EqualityComparer<RootId>.Default.Equals(parentId(c),rootId))){
                yield return new TreeNode<TEntity>{
                    Item = item,
                    Children = collection.GenerateTree(id,parentId,id(item))
                };
            }
        }

         public static IEnumerable<TEntity> GenerateITree<TEntity,RootId> (
            this IEnumerable<TEntity> collection,
            Func<TEntity,RootId> id_selector,
            Func<TEntity,RootId> parentId_selector,
            RootId rootId=default(RootId)

            )  where TEntity :ITreeNode<TEntity> {

            foreach(var item in collection.Where(c => EqualityComparer<RootId>.Default.Equals(parentId_selector(c),rootId))){
                item.Children =  collection.GenerateITree(id_selector,parentId_selector,id_selector(item));
                yield return item;
            }
        }
    }
}