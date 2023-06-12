import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { map, startWith } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'di-mat-chip',
  templateUrl: './di-mat-chip.component.html',
  styleUrls: ['./di-mat-chip.component.css']
})
export class DiMatChipComponent implements OnInit {
  constructor() {
    this.filteredDataList = this.dataCtrl.valueChanges.pipe(
      startWith(null),
      map((data: string | null) => data ? this._filter(data) : (this.allDataList && this.allDataList.length > 0 ? this.allDataList.slice() : this.allDataList)));
  }

  ngOnInit(): void {
  }
  @Input() label = "Label";
  @Input() visible = true;
  @Input() selectable = true;
  @Input() removable = true;
  @Input() datalist: string[] = [];
  @Input() allDataList: string[] = [];

  @Output() datalistChange = new EventEmitter<string[]>();

  separatorKeysCodes: number[] = [ENTER, COMMA];
  dataCtrl = new FormControl();
  filteredDataList: Observable<string[]>;
  @ViewChild('dataInput') dataInput!: ElementRef<HTMLInputElement>;
  @ViewChild('auto') matAutocomplete!: MatAutocomplete;


  add(event: MatChipInputEvent): void {
    const value = (event.value || '').trim();
    // Add our data
    if (value) {
      this.datalist.push(value);
      this.allDataList.push(value);
      this.datalistChange.emit(this.datalist);
    }

    // Clear the input value
    event.chipInput!.clear();

    this.dataCtrl.setValue(null);
  }

  remove(data: string): void {
    const index = this.datalist.indexOf(data);

    if (index >= 0) {
      this.datalist.splice(index, 1);
      this.datalistChange.emit(this.datalist);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.datalist.push(event.option.viewValue);
    this.dataInput.nativeElement.value = '';
    this.dataCtrl.setValue(null);
    this.datalistChange.emit(this.datalist);
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.allDataList.filter(data => data.toLowerCase().indexOf(filterValue) === 0);
  }
}
