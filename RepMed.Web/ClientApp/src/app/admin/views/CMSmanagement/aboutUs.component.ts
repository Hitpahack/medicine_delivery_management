import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { CKEditorModule } from '@ckeditor/ckeditor5-angular'; // Add this if standalone
import { AdminBaseComponent } from "../../admin.base.component";
import { AutoValidateDirective } from '../../../common/form.validator';

@Component({
    selector: 'admin-cms-management',
    templateUrl: './aboutUs.component.html',
    styleUrl: './common.component.css',
    standalone: true,
    imports: [CKEditorModule, ReactiveFormsModule, AutoValidateDirective],
})

export class AboutUSCmsComponent extends AdminBaseComponent implements OnInit {
    cmsForm: FormGroup;
    pageTitle: string = 'AboutUS Page';

    constructor(
        public fb: FormBuilder, 
        private route: ActivatedRoute
    ) { super(); }

    ngOnInit(): void {
        const pageType = this.route.snapshot.data['pageType'];
        this.pageTitle = pageType || 'AboutUS Page';

        this.cmsForm = this.fb.group({
            title: ['', [Validators.required, Validators.maxLength(100)]],
            description: ['', Validators.required]
        });

        // If editing existing page, load data
        this.loadCmsData(pageType);
    }

    loadCmsData(pageType: string) {
        // API Call here to fetch existing data by pageType if needed
        // Example response patch:
        /*
        this.cmsForm.patchValue({
            title: 'About Us',
            description: '<p>This is About Us content</p>'
        });
        */
    }

    onSubmit() {
        if (this.cmsForm.valid) {
            const cmsData = this.cmsForm.value;
            console.log('CMS Data:', cmsData);
            // Call your API to save data
        }
    }
}
