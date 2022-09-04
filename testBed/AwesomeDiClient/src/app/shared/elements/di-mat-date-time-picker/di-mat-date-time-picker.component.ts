import { Component, OnInit, ViewChild, Input, Output, EventEmitter  } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ThemePalette } from '@angular/material/core';
import * as moment from 'moment';

@Component({
  selector: 'di-mat-date-time-picker',
  templateUrl: './di-mat-date-time-picker.component.html',
  styleUrls: ['./di-mat-date-time-picker.component.css']
})
export class DiMatDateTimePickerComponent implements OnInit {
  @ViewChild('picker') picker: any;

  @Input() label = "Date time";
  @Input() disabled = false;
  @Input() showSpinners = true;
  @Input() showSeconds = true;
  @Input() touchUi = false;
  @Input() enableMeridian = false;
  @Input() minDate: Date|null = null;
  @Input() maxDate: Date|null = null;
  @Input() stepHour = 1;
  @Input() stepMinute = 1;
  @Input() stepSecond = 1;
  @Input() color: ThemePalette = 'primary';
  @Input() disableMinute = false;
  @Input() hideTime = false;
  @Input() value: Date|null = null;
  @Output() valueChange = new EventEmitter();

  public dateControl = new FormControl(null);
  constructor() { }

  ngOnInit(): void {
    this.dateControl.setValue(this.value);
    this.dateControl.valueChanges.subscribe(value =>{
      this.value = value;
      this.valueChange.emit(this.value);
    });
  }


}
