import { ILoginFormModel } from './core/log-in-screen/log-in-screen.component';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'your-project-name';

  isLoggined: boolean = false;

  onFormSubmitted(data: ILoginFormModel) {
    this.isLoggined = true;
  }
}
