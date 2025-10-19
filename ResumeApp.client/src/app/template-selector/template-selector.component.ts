import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-template-selector',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './template-selector.component.html',
  styleUrls: ['./template-selector.component.css']
})
export class TemplateSelectorComponent {
  templates = [
    {
      id: 'modern',
      name: 'Modern',
      description: 'Clean and professional design'
    },
    {
      id: 'classic',
      name: 'Classic',
      description: 'Traditional layout with elegant typography'
    },
    {
      id: 'creative',
      name: 'Creative',
      description: 'Bold and artistic presentation'
    }
  ];

  selectedTemplate = 'modern';

  selectTemplate(templateId: string) {
    this.selectedTemplate = templateId;
    console.log('Selected template:', templateId);
  }
}
