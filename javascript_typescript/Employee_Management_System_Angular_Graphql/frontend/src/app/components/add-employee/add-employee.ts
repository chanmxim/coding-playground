import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Apollo } from 'apollo-angular';
import { ADD_NEW_EMPLOYEE } from '../../graphql/operations';

@Component({
  selector: 'app-add-employee',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './add-employee.html',
})
export class AddEmployee {
  private fb = inject(FormBuilder);
  private apollo = inject(Apollo);
  private router = inject(Router);

  isLoading = signal(false);
  photoPreview = signal<string | null>(null);

  employeeForm = this.fb.group({
    first_name: ['', [Validators.required]],
    last_name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    gender: ['Male', [Validators.required]],
    designation: ['', [Validators.required]],
    salary: [0, [Validators.required, Validators.min(0)]],
    date_of_joining: ['', [Validators.required]],
    department: ['', [Validators.required]],
    employee_photo: ['']
  });

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      if (file.size > 2000000) { // 2MB limit
        alert("File is too large. Please select an image under 2MB.");
        return;
      }

      const reader = new FileReader();
      reader.onload = () => {
        const base64String = reader.result as string;
        this.photoPreview.set(base64String);
        this.employeeForm.patchValue({ employee_photo: base64String });
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit() {
    if (this.employeeForm.valid) {
      this.isLoading.set(true);
      
      const rawValues = this.employeeForm.value;

      this.apollo.mutate({
        mutation: ADD_NEW_EMPLOYEE,
        variables: {
          first_name: rawValues.first_name,
          last_name: rawValues.last_name,
          email: rawValues.email,
          gender: rawValues.gender,
          designation: rawValues.designation,
          salary: Number(rawValues.salary),
          date_of_joining: rawValues.date_of_joining,
          department: rawValues.department,
          employee_photo: rawValues.employee_photo || "" 
        }
      }).subscribe({
        next: () => {
          this.router.navigate(['/employees']);
        },
        error: (err) => {
          this.isLoading.set(false);
          console.error(err);
        }
      });
    }
  }
}
