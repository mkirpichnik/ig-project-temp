import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LogInScreenComponent } from './log-in-screen/log-in-screen.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
  declarations: [
    LogInScreenComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    BrowserModule
  ],
  exports: [
    LogInScreenComponent
  ]
})
export class CoreModule { }
