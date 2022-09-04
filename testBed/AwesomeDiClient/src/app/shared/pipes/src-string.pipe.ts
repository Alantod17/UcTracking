import { CommonService } from './../services/common.service';
import { Pipe, PipeTransform } from '@angular/core';
@Pipe({ name: 'srcString' })
export class SrcStringPipe implements PipeTransform {
    constructor(private commonService: CommonService) { }
    transform(value: string): string {
        return `${value}/${this.commonService.getCurrentUser()?.accessToken}`;
    }
}