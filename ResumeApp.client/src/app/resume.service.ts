
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Resume {
  id: number;
  headline: string;
  summary: string;
  createdAt: string;
  updatedAt: string;
  personId: number;
}

export interface ResumeListResponse {
  total: number;
  items: Resume[];
}

export interface ResumePayload {
  headline: string;
  summary: string;
}

@Injectable({
  providedIn: 'root'
})
export class ResumeService {
  private readonly apiUrl = '/api/resumes';

  constructor(private http: HttpClient) {}

  getResumes(page: number, size: number): Observable<ResumeListResponse> {
    return this.http.get<ResumeListResponse>(`${this.apiUrl}?page=${page}&size=${size}`);
  }

  getResume(id: number): Observable<Resume> {
    return this.http.get<Resume>(`${this.apiUrl}/${id}`);
  }

  createResume(resume: ResumePayload): Observable<Resume> {
    return this.http.post<Resume>(this.apiUrl, resume);
  }

  updateResume(id: number, resume: ResumePayload): Observable<Resume> {
    return this.http.put<Resume>(`${this.apiUrl}/${id}`, resume);
  }

  deleteResume(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
