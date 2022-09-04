import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import * as moment from 'moment';

@Component({
  selector: 'di-mat-date-picker',
  templateUrl: './di-mat-date-picker.component.html',
  styleUrls: ['./di-mat-date-picker.component.css']
})
export class DiMatDatePickerComponent implements OnInit {
  @Input() matDatepicker: string | null = null;
  @Input() max: string | null = null;
  @Input() min: string | null = null;
  @Input() placeholder: string = "";
  @Input() value: string | null = null;
  @Output() valueChange = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  valueChanged(newVal: any) {
    this.value = newVal;
    this.valueChange.emit(moment(this.value).utc().format());
  }
}
