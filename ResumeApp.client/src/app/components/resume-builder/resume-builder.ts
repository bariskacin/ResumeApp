import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Resume, ResumePayload, ResumeService } from '../../resume.service';

@Component({
  selector: 'app-resume-builder',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './resume-builder.html',
  styleUrl: './resume-builder.css'
})
export class ResumeBuilder implements OnInit {
  protected readonly resumes = signal<Resume[]>([]);
  protected readonly total = signal(0);
  protected readonly loading = signal(false);
  protected readonly saving = signal(false);
  protected readonly selectedResumeId = signal<number | null>(null);
  protected readonly errorMessage = signal<string | null>(null);

  private readonly fb = inject(FormBuilder);

  protected readonly resumeForm = this.fb.nonNullable.group({
    headline: ['', [Validators.required, Validators.maxLength(500)]],
    summary: ['', [Validators.maxLength(2000)]]
  });

  constructor(
    private readonly resumeService: ResumeService
  ) {}

  ngOnInit(): void {
    this.loadResumes();
  }

  protected loadResumes(): void {
    this.loading.set(true);
    this.errorMessage.set(null);

    this.resumeService.getResumes(1, 20).subscribe({
      next: (response) => {
        this.resumes.set(response.items);
        this.total.set(response.total);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.errorMessage.set('Unable to load resumes. Please try again.');
      }
    });
  }

  protected editResume(resume: Resume): void {
    this.selectedResumeId.set(resume.id);
    this.resumeForm.setValue({
      headline: resume.headline,
      summary: resume.summary ?? ''
    });
  }

  protected deleteResume(resume: Resume): void {
    if (!confirm(`Delete "${resume.headline}"?`)) {
      return;
    }

    this.loading.set(true);
    this.resumeService.deleteResume(resume.id).subscribe({
      next: () => this.loadResumes(),
      error: () => {
        this.loading.set(false);
        this.errorMessage.set('Unable to delete the resume. Please try again.');
      }
    });
  }

  protected resetForm(): void {
    this.selectedResumeId.set(null);
    this.resumeForm.reset({
      headline: '',
      summary: ''
    });
  }

  protected submit(): void {
    if (this.resumeForm.invalid) {
      this.resumeForm.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    this.errorMessage.set(null);
    const payload: ResumePayload = this.resumeForm.getRawValue();
    const request$ = this.selectedResumeId()
      ? this.resumeService.updateResume(this.selectedResumeId()!, payload)
      : this.resumeService.createResume(payload);

    request$.subscribe({
      next: () => {
        this.saving.set(false);
        this.resetForm();
        this.loadResumes();
      },
      error: () => {
        this.saving.set(false);
        this.errorMessage.set('Unable to save the resume. Please try again.');
      }
    });
  }
}
