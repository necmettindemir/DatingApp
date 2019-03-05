import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';

import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})

export class MemberEditComponent implements OnInit {

  user: User;
  photoUrl: string;

  @ViewChild('editForm') editForm: NgForm;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }



  // constructor(private route: ActivatedRoute) { }


  constructor(private userService: UserService,
              private authService: AuthService,
              private route: ActivatedRoute,
              private alertify: AlertifyService) {}



    ngOnInit() {

      this.route.data.subscribe(data => {
         // this.user = data['user'];
          this.user = data.user;
      });

     // this.loadUser();

      // this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
      this.authService.currentPhotoUrl.subscribe(p => this.photoUrl = p);
    }


    //  loadUser() {
    //   this.userService.getUser(this.authService.decodedToken.nameid)
    //               .subscribe( (user: User) => {
    //                   this.user = user;
    //               },
    //               error => {
    //                   this.alertify.error(error);
    //               });

    // }


    updateUser() {
      try {
        // console.log(this.user);

        this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {

        this.alertify.success('profile updated successfully!');
        this.editForm.reset(this.user);

        },
        error => {
          this.alertify.error(error);
        }
        );
        /*
        this.alertify.success('profile updated successfully!');
        this.editForm.reset(this.user);
        */

      } catch (error) {
        this.alertify.error('opps! ' + error.error);
      }

    }

    updateMainPhoto(photoUrl) {
      this.user.photoUrl = photoUrl;
    }

}
