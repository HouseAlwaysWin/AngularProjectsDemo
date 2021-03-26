

export function StringFormat(
  text: string, ...args): string {
  return text.replace(/{(\d+)}/g, function (match, number) {
    return typeof args[number] != 'undefined'
      ? args[number]
      : match
      ;
  });
}

