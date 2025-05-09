import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'; // Add this if standalone
import { AdminBaseComponent } from "../../admin.base.component";
import { AutoValidateDirective } from '../../../common/form.validator';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { FAQService } from '../../../admin/services/cms/faq.services';
import { Helper } from "../../../../app/common/helper.extenstions";
import { FAQDto } from "../../../viewmodels/cms/faq.dto";
import { CustomValidator } from "../../../common/custom.validators";

@Component({
    selector: 'admin-faq-management',
    templateUrl: './faq.component.html',
    styleUrl: './common.component.css',
    standalone: true,
    imports: [CKEditorModule, ReactiveFormsModule, AutoValidateDirective],
})

export class FAQComponent extends AdminBaseComponent implements OnInit {
    FAQForm: FormGroup;
    pageTitle: string = 'Add';
    public Editor = ClassicEditor;
    pageType: string;
    isEditMode: boolean = false;
    errorMessage: string = '';
    FAQData: FAQDto | null = null;

    constructor(
        public fb: FormBuilder,
        private route: ActivatedRoute,
        private cd: ChangeDetectorRef,
        public FAQService: FAQService,
        public router: Router,
        public validator: CustomValidator
    ) { super(); }

    ngOnInit(): void {
        const pageType = this.route.snapshot.data['pageType'];
        this.pageTitle = pageType || 'Add';

        this.FAQForm = this.fb.group({
            Question: ['', [Validators.required, Validators.maxLength(200)]],
            Answer: ['', [Validators.required]],
        });
        // Check if we are editing
        const id = this.route.snapshot.params['id']; // Assuming URL has /FAQ/:id for edit
        if (id) {
            this.isEditMode = true;
            this.loadfaqData(id);
        }
    }

    loadfaqData(id: number) {
        this.FAQService.getbyid(id).subscribe({
            next: (response) => {
                if (response?.isSuccess && response.data) {
                    const data = response.data;
                    this.FAQData = data;
                    console.log('this is faq', data)
                    this.FAQForm.patchValue({
                        Question: data.question ?? '',
                        Answer: data.answer ?? '',
                    });
                    this.cd.detectChanges();
                } else {
                    console.error("Failed to load FAQ data", response);
                    Helper.ShowError(response.message || "Failed to load FAQ data.");
                }
            },
            error: (err) => {
                console.error("API Error:", err);
                Helper.ShowError(err?.error?.message || "Something went wrong while loading data.");
            }
        });
    }

    onSubmit() {
        if (this.FAQForm.invalid) {
            this.validator.markInvalidFieldsTouched(this.FAQForm);
            return;
        }

        //const FAQ = this.FAQ.value;
        const dto: FAQDto = this.FAQForm.value;

        if (this.isEditMode) {
            const id = this.route.snapshot.params['id'];
            this.FAQService.edit(dto, id).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'FAQ page updated successfully.');
                        this.router.navigate(['/admin/faq/list']);
                    } else {
                        console.error('Update failed:', response);
                        Helper.ShowError(response.message || 'Failed to update FAQ page.');
                    }
                },
                error: (err) => {
                    console.error('HTTP Error:', err);
                    Helper.ShowError(err?.error?.message || 'Something went wrong. Please try again.');
                }
            });
        } else {
            this.FAQService.add(dto).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'FAQ page added successfully.');
                        this.router.navigate(['/admin/faq/list']);
                    } else {
                        console.error('Add failed:', response);
                        Helper.ShowError(response.message || 'Failed to add FAQ page.');
                    }
                },
                error: (err) => {
                    console.error('HTTP Error:', err);
                    Helper.ShowError(err?.error?.message || 'Something went wrong. Please try again.');
                }
            });
        }
    }
}