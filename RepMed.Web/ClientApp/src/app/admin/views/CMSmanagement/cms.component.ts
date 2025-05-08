import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'; // Add this if standalone
import { AdminBaseComponent } from "../../admin.base.component";
import { AutoValidateDirective } from '../../../common/form.validator';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { CMSService } from '../../../admin/services/cms/cms.services';
import { Helper } from "../../../../app/common/helper.extenstions";
import { CMSSDto } from "../../../viewmodels/cms/cms.dto";

@Component({
    selector: 'admin-cms-management',
    templateUrl: './cms.component.html',
    styleUrl: './common.component.css',
    standalone: true,
    imports: [CKEditorModule, ReactiveFormsModule, AutoValidateDirective],
})

export class CmsComponent extends AdminBaseComponent implements OnInit {
    cmsForm: FormGroup;
    pageTitle: string = 'Add Page';
    public Editor = ClassicEditor;
    pageType: string;
    isEditMode: boolean = false;
    errorMessage: string = '';
    cmsData: CMSSDto | null = null;

    constructor(
        public fb: FormBuilder,
        private route: ActivatedRoute,
        private cd: ChangeDetectorRef,
        public CMSService: CMSService,
        public router: Router
    ) { super(); }

    ngOnInit(): void {
        const pageType = this.route.snapshot.data['pageType'];
        this.pageTitle = pageType || 'Add Page';

        this.cmsForm = this.fb.group({
            title: ['', [Validators.required, Validators.maxLength(100)]],
            slug: ['', [Validators.required, Validators.maxLength(100)]],
            content: ['', Validators.required]
        });
        // Check if we are editing
        const id = this.route.snapshot.params['id']; // Assuming URL has /cms/:id for edit
        if (id) {
            this.isEditMode = true;
            this.loadCmsData(id);
        }
    }

    loadCmsData(id: number) {
        this.CMSService.getbyid(id).subscribe({
            next: (response) => {
                if (response?.isSuccess && response.data) {
                    const data = response.data;
                    this.cmsData = data;
                    this.cmsForm.patchValue({
                        title: data.title,
                        slug: data.slug,
                        content: data.content
                    });
                    this.cd.detectChanges();
                } else {
                    console.error("Failed to load CMS data", response);
                    Helper.ShowError(response.message || "Failed to load CMS data.");
                }
            },
            error: (err) => {
                console.error("API Error:", err);
                Helper.ShowError(err?.error?.message || "Something went wrong while loading data.");
            }
        });
    }

    onSubmit() {
        if (this.cmsForm.invalid) {
            Helper.ShowError("Please fill in all required fields.");
            return;
        }

        //const cmsData = this.cmsForm.value;
        const dto: CMSSDto = this.cmsForm.value;

        if (this.isEditMode) {
            const id = this.route.snapshot.params['id'];
            this.CMSService.edit(dto, id).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'CMS page updated successfully.');
                        this.router.navigate(['/admin/cms/list']);
                    } else {
                        console.error('Update failed:', response);
                        Helper.ShowError(response.message || 'Failed to update CMS page.');
                    }
                },
                error: (err) => {
                    console.error('HTTP Error:', err);
                    Helper.ShowError(err?.error?.message || 'Something went wrong. Please try again.');
                }
            });
        } else {
            this.CMSService.add(dto).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        Helper.ShowSuccess(response.message || 'CMS page added successfully.');
                        this.router.navigate(['/admin/cms/list']);
                    } else {
                        console.error('Add failed:', response);
                        Helper.ShowError(response.message || 'Failed to add CMS page.');
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
