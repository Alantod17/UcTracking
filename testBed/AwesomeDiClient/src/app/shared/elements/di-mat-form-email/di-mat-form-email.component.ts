import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { CommonService } from '../../services/common.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'di-mat-form-email',
  templateUrl: './di-mat-form-email.component.html',
  styleUrls: ['./di-mat-form-email.component.css']
})
export class DiMatFormEmailComponent implements OnInit {
  @Input() placeholder: string  = "";
  @Input() name: string = "";
  @Input() autocomplete: string | null = null;
  @Input() value: string | null = null;
  @Output() valueChange = new EventEmitter();
  @Input() error: string | null = null;

  constructor(private commonService: CommonService) {
  }

  ngOnInit() {
    if (!this.placeholder) this.placeholder = "Email";
    if (!this.name) this.name = this.placeholder.toLowerCase().replace(" ", "-");
    if (!this.autocomplete) this.autocomplete = this.placeholder.toLowerCase().replace(" ", "");
    if (!this.commonService.hasValue(this.value)) this.value = null;
  }

  valueChanged(event:any) {
    let newVal = event.target.value;
    this.value = newVal;
    this.valueChange.emit(this.value);
  }

}
