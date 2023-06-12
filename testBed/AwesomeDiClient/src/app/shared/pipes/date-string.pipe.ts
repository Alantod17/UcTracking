import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
@Pipe({ name: 'dateStringPipe' })
export class DateStringPipe implements PipeTransform {
    transform(value: any): string {
        return moment(value).utc().format();
    }
}