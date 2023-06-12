import { Directive, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { NgModel } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, skip } from 'rxjs/operators';

@Directive({
  selector: '[ngModelDebounceChange]',
})
export class NgModelDebounceChangeDirective implements OnDestroy, OnInit {
  /** Emit event when model has changed. */
  @Output() ngModelDebounceChange = new EventEmitter<any>();

  private subscription: Subscription = new Subscription;

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  constructor(private ngModel: NgModel) { }

  ngOnInit(): void {
    this.subscription = this.ngModel.control.valueChanges
      .pipe(skip(1), debounceTime(200), distinctUntilChanged())
      .subscribe(value => this.ngModelDebounceChange.emit(value));
  }
}
