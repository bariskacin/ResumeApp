import { Routes } from '@angular/router';
import { ResumeEditorComponent } from './resume-editor/resume-editor.component';
import { PreviewComponent } from './preview/preview.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/editor',
    pathMatch: 'full'
  },
  {
    path: 'editor',
    component: ResumeEditorComponent
  },
  {
    path: 'preview',
    component: PreviewComponent
  }
];
