import { GoogleLoginParameter, GoogleLoginResult } from './../../shared/services/dto';
import { DataService } from './../../shared/services/data.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { GoogleLoginProvider, SocialAuthService } from 'angularx-social-login';
import { LoginParameter, LoginResult } from 'src/app/shared/services/dto';
import { CommonService } from 'src/app/shared/services/common.service';
import { EventBrokerService } from 'src/app/shared/services/event.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(private authService: SocialAuthService, private activatedRoute: ActivatedRoute, private dataService: DataService, private commonService: CommonService, private eventBroker: EventBrokerService, private router: Router) {
  }
  ngOnInit(): void {
    this.redirectUrl = this.activatedRoute.snapshot.queryParams.redirectUrl;
    let dId = this.commonService.loadLocalData("deviceId");
    if (this.commonService.hasValue(dId)) {
      this.deviceId = dId;
    } else {
      this.deviceId = this.commonService.newGuid();
      this.commonService.setLocalData("deviceId", this.deviceId);
    }
  }
  deviceId: string | null = null;
  redirectUrl = null;
  loginParam: LoginParameter = new LoginParameter();
  email = new FormControl('', [Validators.required, Validators.email]);
  hide = true;
  loading = false;

  login() {
    this.loginParam.deviceId = this.deviceId;
    this.dataService.login(this.loginParam)
      .subscribe(
        res => {
          this.processLoginAndSignUp(res);
        },
        err => {
          this.commonService.notifyErrs(err);

        },
        () => {
        }
      )
  }

  async loginGoogle(): Promise<void> {
    try {
      this.loading = true;
      let googleRes = await this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
      let param = new GoogleLoginParameter();
      param.email = googleRes.email;
      param.idToken = googleRes.idToken;
      param.deviceId = this.deviceId
      let loginRes = await this.dataService.googleLogin(param).toPromise();
      this.processLoginAndSignUp(loginRes);
    } finally {
      this.loading = false;
    }

    // this.authService.signIn(GoogleLoginProvider.PROVIDER_ID).then(res => { 
    //   let param = new GoogleLoginParameter();
    //   param.email = res.email;
    //   param.idToken = res.idToken;
    //   this.dataService.googleLogin(param)
    // });
  }

  private processLoginAndSignUp(res: LoginResult | GoogleLoginResult | undefined) {
    if (!res || !this.commonService.hasValue(res.accessToken)) {
      this.commonService.notifyErr("Login Failed");
      return;
    }
    this.commonService.setLocalData("currentUser", res);
    this.eventBroker.emit<LoginResult>("di-login-success", res);
    if (this.commonService.hasValue(this.redirectUrl)) {
      this.router.navigate([this.redirectUrl]);
    } else {
      this.router.navigate(['']);
    }
  }

}
