import { SearchSharesiesInstrumentParameter, SearchSharesiesInstrumentPagedResult, SearchSharesiesInstrumentResult } from './../../shared/services/dto';
import { AfterViewInit, Component, EventEmitter, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { lastValueFrom, merge, of } from 'rxjs';
import { catchError, debounceTime, map, mergeAll, skip, startWith, switchMap } from 'rxjs/operators';
import { DataService } from 'src/app/shared/services/data.service';
import { MatDialog } from '@angular/material/dialog';
import { DiMatIframeDialogComponent } from 'src/app/shared/elements/di-mat-iframe-dialog/di-mat-iframe-dialog.component';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewInit, OnChanges {
  keywords: string | null = null;
  country: string | null = null;
  resultsLength = 0;
  isLoadingResults = true;
  displayedColumns: string[] = ['name', 'exchangeCountry', 'lastPrice', 'dayChangePercent', 'threeDayChangePercent', 'weekChangePercent', 'twoWeekChangePercent',
  "monthChangePercent","twoMonthChangePercent","threeMonthChangePercent","sixMonthChangePercent","yearChangePercent","actions"
];
  dataSource: MatTableDataSource<SearchSharesiesInstrumentResult> = new MatTableDataSource();
  @ViewChild(MatSort) sort: MatSort = {} as MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator = {} as MatPaginator;
  filterChanged = new EventEmitter<any>();
  
  constructor(private dataService: DataService,public dialog: MatDialog) { }
  ngOnChanges(changes: SimpleChanges): void {
    // throw new Error('Method not implemented.');
  }
  countryChanged(newVal: string) {
    this.country = newVal;
    this.filterChanged.emit();
  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    of(this.sort.sortChange, this.paginator.page, this.filterChanged)
      .pipe(
        mergeAll(),
        startWith({}),
        debounceTime(500),
        switchMap(() => {
          this.isLoadingResults = true;
          let param: SearchSharesiesInstrumentParameter = {
            sortField: this.sort.active ? this.sort.active : null,
            sortDirection: this.sort.direction ? this.sort.direction : null,
            keywords: this.keywords,
            exchangeCountry: this.country,
            skip: this.paginator.pageSize * this.paginator.pageIndex,
            take: this.paginator.pageSize
          };
          return this.dataService.searchSharesiesInstrument(param);
        }),
        map(data => {
          // Flip flag to show that loading has finished.
          this.isLoadingResults = false;
          this.resultsLength = data.totalCount;
          return data.results;
        }),
        catchError(() => {
          setTimeout(() => {
            this.isLoadingResults = false;
          }, 300);
          return [];
        })
      )
      .subscribe(data => {
        this.dataSource.data = data as SearchSharesiesInstrumentResult[]
      });
  }
  async view(ra: SearchSharesiesInstrumentResult) {
    // const dialogRef = this.dialog.open(DiMatIframeDialogComponent, {
    //   width: '250px',
    //   data: {src: `https://app.sharesies.com/portfolio/${ra.urlSlug}`},
    // });

    // dialogRef.afterClosed().subscribe(result => {
    // });
    let url = `https://app.sharesies.com/portfolio/${ra.urlSlug}`
    window.open(url, '_blank');
  }
  
}
