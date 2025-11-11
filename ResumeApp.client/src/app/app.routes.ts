import { Routes } from '@angular/router';
import { AuthComponent } from './components/auth/auth.component';
import { LandingComponent } from './components/landing/landing.component';
import { ResumeBuilder } from './components/resume-builder/resume-builder';
import { authGuard } from './auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: LandingComponent
  },
  {
    path: 'auth',
    component: AuthComponent
  },
  {
    path: 'builder',
    component: ResumeBuilder,
    canActivate: [authGuard]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
