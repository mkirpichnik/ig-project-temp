import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostsDashboardComponent } from './posts-dashboard/posts-dashboard.component';

@NgModule({
  declarations: [PostsDashboardComponent],
  imports: [
    BrowserModule,
    CommonModule
  ],
  exports: [
    PostsDashboardComponent
  ]
})
export class DashboardModule { }
