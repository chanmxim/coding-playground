import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Apollo } from 'apollo-angular';
import { GET_EMPLOYEE_BY_ID, UPDATE_EMPLOYEE } from '../../graphql/operations';
import { Employee } from '../../models/Employee';

@Component({
  selector: 'app-edit-employee',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './edit-employee.html',

})
export class EditEmployee implements OnInit{
  private fb = inject(FormBuilder);
  private apollo = inject(Apollo);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  isLoading = signal(false);
  isFetching = signal(true);
  photoPreview = signal<string | null>(null);
  employeeId: string | null = null;

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

  ngOnInit() {
    this.employeeId = this.route.snapshot.paramMap.get('id');
    
    if (this.employeeId) {
      this.fetchEmployeeData();
    }
  }

  fetchEmployeeData() {
    this.apollo.query<any>({
      query: GET_EMPLOYEE_BY_ID,
      variables: { id: this.employeeId },
      fetchPolicy: 'network-only'
    }).subscribe({
    next: (result) => {
      const emp: Employee = result.data.searchEmployeeByEid;
      if (emp) {
        let formattedDate = '';
        
        if (emp.date_of_joining) {
          const isNumeric = /^\d+$/.test(emp.date_of_joining);

          const dateObj = new Date(isNumeric ? parseInt(emp.date_of_joining, 10) : emp.date_of_joining);
          
          if (!isNaN(dateObj.getTime())) {
            const year = dateObj.getUTCFullYear();
            const month = String(dateObj.getUTCMonth() + 1).padStart(2, '0');
            const day = String(dateObj.getUTCDate()).padStart(2, '0');
            formattedDate = `${year}-${month}-${day}`;
          }
        }
        
        this.employeeForm.patchValue({
          first_name: emp.first_name,
          last_name: emp.last_name,
          email: emp.email,
          gender: emp.gender,
          designation: emp.designation,
          salary: emp.salary,
          department: emp.department,
          employee_photo: emp.employee_photo,
          date_of_joining: formattedDate
        });
        
        if (emp.employee_photo) {
          this.photoPreview.set(emp.employee_photo);
        }
      }
      this.isFetching.set(false);
    },
      error: (err) => {
        console.error(err);
        this.isFetching.set(false);
      }
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
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
    if (this.employeeForm.valid && this.employeeId) {
      this.isLoading.set(true);
      const rawValues = this.employeeForm.value;

      this.apollo.mutate({
        mutation: UPDATE_EMPLOYEE,
        variables: {
          id: this.employeeId,
          first_name: rawValues.first_name,
          last_name: rawValues.last_name,
          email: rawValues.email,
          gender: rawValues.gender,
          designation: rawValues.designation,
          salary: Number(rawValues.salary),
          department: rawValues.department,
          employee_photo: rawValues.employee_photo
        }
      }).subscribe({
        next: () => this.router.navigate(['/employees']),
        error: (err) => {
          this.isLoading.set(false);
          alert(err.message);
        }
      });
    }
  }
}
