import { Component, OnInit } from '@angular/core';
import { SwUpdate } from '@angular/service-worker';
import { switchAnimation } from './shared/services/animation';
import { CommonService } from './shared/services/common.service';
import { LoginResult } from './shared/services/dto';
import { EventBrokerService } from './shared/services/event.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [switchAnimation], // register the animation
})
export class AppComponent implements OnInit {
  hasUpdates = false;
  isCollapsed = true;
  email: string | undefined | null = null;

  show = true;
  constructor(private swUpdate: SwUpdate, private eventBroker: EventBrokerService, private commonService: CommonService) {
    this.eventBroker.listen<LoginResult>("di-login-success", () => {
      this.email = (this.commonService.loadLocalData("currentUser") as LoginResult).email;
    });
    this.eventBroker.listen<LoginResult>("di-logout-success", () => {
      this.email = null;
    });
  }
  reload() {
    document.location.reload()
  }
  logout() {
    this.commonService.logout();
  }

  test() {
  }
  ngOnInit() {
    let user = (this.commonService.loadLocalData("currentUser") as LoginResult);
    if (user != null) this.email = user.email;
    if (this.swUpdate.isEnabled) {
      this.swUpdate.available.subscribe(event => {
        this.hasUpdates = true;
        // this.swUpdate.activateUpdate().then(()=>{});
      });
    }
  }
}
