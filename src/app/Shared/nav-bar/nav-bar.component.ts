import { AuthService } from './../Services/auth.service';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent {


  userName: string | null = null;

ngOnInit(): void {
  this.userName = this.authService.getUserName();
}
 constructor(private authService:AuthService , private router: Router){}
  logout() {
  this.authService.logout();          
  this.router.navigate(['/login']);   
}
}
