import { animate, query, stagger, state, style, transition, trigger } from "@angular/animations";

export function FadeInGrowListAnimation(
  staggerTime: number = 50,
  animateTime: number = 500,
  triggerName: string = 'fadeInGrowList'
) {
  return trigger(triggerName, [
    transition(':enter', [
      query(':enter', [
        style({ opacity: 0 }),
        stagger(`${staggerTime}ms`, [
          animate(`${animateTime}ms`, style({ opacity: 1 }))
        ])
      ])
    ])
  ])
}

export function RotatedAnimation(time: number = 300, startDeg: number = 0, endDeg: number = 90, triggerName: string = 'rotated') {
  return trigger(triggerName, [
    state('start', style({
      transform: `rotate(${startDeg}deg)`
    })),
    state('end', style({
      transform: `rotate(${endDeg}deg)`
    })),
    transition('start <=> end',
      animate(time)
    )
  ])
}
