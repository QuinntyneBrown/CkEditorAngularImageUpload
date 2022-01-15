import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { CKEditor4 } from 'ckeditor4-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  readonly config =  {
    removeDialogTabs :'image:advanced;image:Link;link:advanced;link:upload',
    filebrowserImageUploadUrl: `https://localhost:5001/api/imageUpload?command=upload`,
    extraPlugins: 'colorbutton,colordialog'
  };

  readonly formControl: FormControl = new FormControl(null,[]);

  onReady($event : CKEditor4.EventInfo, formControl: FormControl) {
    $event.editor.on("afterInsertHtml", () => formControl.patchValue($event.editor.getData()));
  }
}
