import { animate, AnimationBuilder, AnimationMetadata, AnimationPlayer, style, transition } from '@angular/animations';
import { Directive, ElementRef, Input, OnInit } from '@angular/core';

@Directive({
  selector: '[fadeInOut]'
})
export class FadeInOutDirective implements OnInit {
  player: AnimationPlayer;
  currentMode: string = 'In';
  time: number = 500;
  constructor(
    private builder: AnimationBuilder,
    private el: ElementRef
  ) { }
  ngOnInit(): void {

  }

  @Input()
  set fadeMode(mode: string) {
    this.currentMode = mode;
  }

  @Input()
  set fadeTime(time: number) {
    this.time = time;
  }

  @Input()
  set fadeShow(show: boolean) {
    console.log(show);
    if (show) {
      if (this.player) {
        this.player.destroy();
      }

      const metadata = (this.currentMode === 'Out') ? this.fadeOut() : this.fadeIn();

      const factory = this.builder.build(metadata);
      const player = factory.create(this.el.nativeElement);
      player.play();
    }
  }


  private fadeIn(): AnimationMetadata[] {
    return [
      style({ opacity: 0 }),
      animate(`${this.time}ms`, style({ opacity: 1 })),
    ]
  }

  private fadeOut(): AnimationMetadata[] {
    return [
      style({ opacity: 1 }),
      animate(`${this.time}ms`, style({ opacity: 0 })),
    ]
  }


}
