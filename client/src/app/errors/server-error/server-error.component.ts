import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css'],
})
export class ServerErrorComponent implements OnInit {
  error: {
    details: string;
    message: string;
    statusCode: number;
  };

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras.state?.error;
    console.log(navigation?.extras.state);
    console.log(this.error);
  }

  ngOnInit(): void {}
}
