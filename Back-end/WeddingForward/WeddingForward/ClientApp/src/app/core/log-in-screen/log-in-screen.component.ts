import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-log-in-screen',
  templateUrl: './log-in-screen.component.html',
  styleUrls: ['./log-in-screen.component.scss']
})
export class LogInScreenComponent implements OnInit {

  loginForm: FormGroup = new FormGroup({
    'login': new FormControl(null, [Validators.required, Validators.pattern("[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$")]),
    'pass': new FormControl(null, [Validators.required]),
  });

  model: ILoginFormModel = {
    login: null,
    password: null
  };

  @Output()
  onFormSubmitted: EventEmitter<ILoginFormModel> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  login() {
    this.onFormSubmitted.emit(this.model);
  }
}

export interface ILoginFormModel {
  login: string;
  password: string;
}
