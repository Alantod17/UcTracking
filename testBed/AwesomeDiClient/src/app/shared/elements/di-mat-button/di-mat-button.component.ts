import { Component, OnInit, EventEmitter, Output, Input, SimpleChanges, SimpleChange } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { MatProgressButtonOptions } from 'mat-progress-buttons';

@Component({
  selector: 'di-mat-button',
  templateUrl: './di-mat-button.component.html',
  styleUrls: ['./di-mat-button.component.css']
})
export class DiMatButtonComponent implements OnInit {
  @Input() loading: boolean = false;
  @Input() disabled: boolean = false;
  @Input() raised: boolean = true;
  @Input() color: ThemePalette = undefined;
  @Input() text: string = "";
  @Output() clicked = new EventEmitter();


  constructor() { }

  ngOnInit() {
    this.btnOpts.disabled = this.disabled;
    this.btnOpts.buttonColor = this.color;
    this.btnOpts.spinnerColor = this.color;
    this.btnOpts.raised = this.raised;
    this.btnOpts.text = this.text;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.loading) {
      this.btnOpts.active = changes.loading.currentValue;
      // this.btnOpts.disabled = changes.loading.currentValue;
    }
  }
  btnOpts: MatProgressButtonOptions = {
    active: false,
    text: 'Button',
    spinnerSize: 19,
    raised: true,
    stroked: true,
    fullWidth: true,
    disabled: false,
    mode: 'indeterminate',
  };
  // Click handler
  btnClick(): void {
    if (!this.btnOpts.disabled && !this.loading) this.clicked.emit();
  }

}
