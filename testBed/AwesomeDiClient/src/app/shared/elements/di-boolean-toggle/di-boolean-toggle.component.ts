import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';

@Component({
  selector: 'di-boolean-toggle',
  templateUrl: './di-boolean-toggle.component.html',
  styleUrls: ['./di-boolean-toggle.component.css']
})
export class DiBooleanToggleComponent implements OnInit {
  @Input() trueText: string = "True";
  @Input() falseText: string = "False";
  @Input() nullText: string = "All";
  @Input() hideFalse: boolean = false;
  @Input() hideNull: boolean = false;
  @Input() value: boolean | null | undefined = null;
  @Output() valueChange = new EventEmitter();
  constructor() { }
  _selectedValue: string = "true"
  get selectedValue(): string {
    return this._selectedValue;
  }
  @Input()
  set selectedValue(value: string) {
    let oldVal = this.value;
    if (value === "true") this.value = true;
    if (value === "false") this.value = false;
    if (value === "null") this.value = null;
    if (oldVal != this.value) {
      this.valueChange.emit(this.value);
      this._selectedValue = value;
    }
  }
  ngOnInit(): void {
    if (this.value === true) {
      if (this._selectedValue != "true") this._selectedValue = "true";
    } else if (this.value === false) {
      if (this._selectedValue != "false") this._selectedValue = "false";
    } else {
      if (this._selectedValue != "null") this._selectedValue = "null";
    }
  }
  ngOnChanges(changes: SimpleChanges) {
    if (changes.value) {
      if (changes.value.currentValue === true) {
        if (this.selectedValue != "true") this.selectedValue = "true";
      } else if (changes.value.currentValue === false) {
        if (this.selectedValue != "false") this.selectedValue = "false";
      } else {
        if (this.selectedValue != "null") this.selectedValue = "null";
      }
    }
  }

}
