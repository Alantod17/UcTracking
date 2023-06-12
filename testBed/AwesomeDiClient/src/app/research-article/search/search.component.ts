import { SearchResearchArticleResult, SearchResearchArticleParameter, SearchExpenseParameter, SearchResearchArticlePagedResult } from './../../shared/services/dto';
import { AfterViewInit, Component, EventEmitter, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { DataService } from 'src/app/shared/services/data.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { lastValueFrom, merge, of } from 'rxjs';
import { catchError, debounceTime, map, mergeAll, skip, startWith, switchMap } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { DetailDialogComponent } from '../detail-dialog/detail-dialog.component';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewInit, OnChanges {

  panelOpenState = false;
  constructor(private dataService: DataService, public dialog: MatDialog) { }
  articaleList: SearchResearchArticleResult[] = new Array<SearchResearchArticleResult>();
  viewType = "Card"
  isDeleted?: boolean | null = false;
  isDuplicate?: boolean | null = null;
  isNeedReview?: boolean | null = false;
  isFilterVisiable: boolean = false;
  filterChanged = new EventEmitter<any>();
  ngOnInit(): void {
    this.loadArticles();
    this.filterChanged.subscribe(() => {
      console.debug("Called");
      while (this.articaleList.length > 0) {
        this.articaleList.pop();
      }
      this.loadArticles();
    })
  }
  private loadArticles(skip = 0) {
    let param: SearchResearchArticleParameter = {
      isDeleted: this.isDeleted ?? undefined,
      isDuplicate: this.isDuplicate ?? undefined,
      isNeedReview: this.isNeedReview ?? undefined,
      skip: skip,
      take: 100
    };
    this.dataService.searchResearchArticle(param).subscribe(res => {
      let list = ([] as SearchResearchArticleResult[]).concat(this.articaleList);
      res.forEach(element => {
        let index = this.articaleList.findIndex(x => x.id == element.id);
        if (index < 0) {
          list.push(element);
        }
      });
      this.articaleList = list;
    });
  }
  ngOnChanges(changes: SimpleChanges) {
    console.debug(changes)
  }
  onScroll() {
    console.debug("Onscroll called")
    if (this.viewType == "Card")
      this.loadArticles(this.articaleList.length);
  }
  public trackItem(index: number, item: any) {
    return item.id;
  }
  scrolledUp() {

  }
  async viewDetail(ra: SearchResearchArticleResult) {
    const infoList = await lastValueFrom(this.dataService.getResearchArticleInfoList());
    this.dialog.open(DetailDialogComponent, {
      data: {
        articleId: ra.id,
        infoList: infoList
      }
    });
  }
  async needReview(ra: SearchResearchArticleResult) {
    ra.isNeedReview = !ra.isNeedReview;
    let res = await this.dataService.updateResearchArticleStatus(ra.id, { ...ra }).toPromise();
  }
  async delete(ra: SearchResearchArticleResult) {
    ra.isDeleted = !ra.isDeleted;
    let res = await this.dataService.updateResearchArticleStatus(ra.id, { ...ra }).toPromise();
  }
  resultsLength = 0;
  isLoadingResults = true;
  displayedColumns: string[] = ['title', 'year', 'actions'];
  dataSource: MatTableDataSource<SearchResearchArticleResult> = new MatTableDataSource();
  @ViewChild(MatSort) sort: MatSort = {} as MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator = {} as MatPaginator;
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
          let param: SearchResearchArticleParameter = {
            sortField: this.sort.active ? this.sort.active : null,
            sortDirection: this.sort.direction ? this.sort.direction : null,
            isDeleted: this.isDeleted,
            isNeedReview: this.isNeedReview,
            Keywords: this.keywords,
            skip: this.paginator.pageSize * this.paginator.pageIndex,
            take: this.paginator.pageSize
          };
          return this.dataService.searchResearchArticlePaged(param);
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
      ).subscribe(data => this.dataSource.data = data as SearchResearchArticleResult[]);
  }
  isDeletedChanged(newVal: boolean) {
    console.debug("isDeletedChanged Changed");
    this.isDeleted = newVal;
    this.filterChanged.emit();
  }

  isNeedReviewChanged(newVal: boolean) {
    this.isNeedReview = newVal;
    this.filterChanged.emit();
  }
  keywords: string | null = null;
  keywordsChanged(newVal: string) {
    this.keywords = newVal;
    this.filterChanged.emit();
  }
}

