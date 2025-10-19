import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-resume-editor',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './resume-editor.component.html',
  styleUrls: ['./resume-editor.component.css']
})
export class ResumeEditorComponent {
  protected readonly title = signal('Resume Editor');
}
