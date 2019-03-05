import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {


  model: any = {};
  photoUrl: string;

  constructor(public authService: AuthService, 
              private alertify: AlertifyService,
              private router: Router
              ) { }

  ngOnInit() {

    this.model.username = 'lopez';
    this.model.password = 'password';

    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }


  login() {
    this.authService.login(this.model).subscribe(next => {
        // console.log('logged in successfuly');
        this.alertify.success('Logged in successfully');

        // -----
       // this.model.username = '';
       // this.model.password = '';
        // -----
      },
      error => {
       // console.log('opps');
        // console.log(error);
        this.alertify.error(error);
      },
      () => {
                this.router.navigate(['/members']);
           }
      );
  }

  loggedin() {
    // const token = localStorage.getItem('token');
    // return !!token;

    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    // console.log('logged out');

    localStorage.removeItem('user');

    this.authService.decodedToken = null;
    this.authService.currentUser = null;

    this.alertify.message('logged out');

    this.router.navigate(['/home']);
  }
}
