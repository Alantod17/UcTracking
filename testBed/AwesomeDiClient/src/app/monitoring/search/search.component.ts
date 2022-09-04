import { SearchTrackingDataParameter, SearchTrackingDataResult } from './../../shared/services/dto';
import { AfterViewInit, Component, EventEmitter, OnChanges, OnInit, ViewChild } from '@angular/core';
import { CommonService } from 'src/app/shared/services/common.service';
import { DataService } from 'src/app/shared/services/data.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { firstValueFrom, of } from 'rxjs';
import { catchError, debounceTime, map, mergeAll, skip, startWith, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewInit {
  startDateTime: Date |null = null;
  endDateTime: Date |null = null;
  trackingId: string |null = null;
  loading: boolean = false;
  displayedColumns: string[] = ['trackingId', 'startDateTime', 'endDateTime','actions'];
  dataSource: MatTableDataSource<SearchTrackingDataResult> = new MatTableDataSource();
  

  constructor(private commonService: CommonService, private dataService: DataService, private router: Router) { }

  ngOnInit(): void {
  }
  async search(){
    try{
      // console.debug("22222222222", this.startDateTime, this.endDateTime);
      this.loading = true;
      let param: SearchTrackingDataParameter= {
        trackingId: undefined,
        startDateTime: undefined,
        endDateTime: undefined
      };
      if(this.startDateTime) param.startDateTime = this.startDateTime.toISOString();
      if(this.endDateTime) param.endDateTime = this.endDateTime.toISOString();
      if(this.trackingId) param.trackingId= this.trackingId;
      this.dataSource.data = await firstValueFrom(this.dataService.searchTrackingData(param));
    }catch(ex){
    }finally{
      this.loading = false;
    }
  }

  ngAfterViewInit() {
  
  }

  
  async view(trackingData: SearchTrackingDataResult) {
    
    this.router.navigate(['monitoring/view'], { state: trackingData});
  }
}
