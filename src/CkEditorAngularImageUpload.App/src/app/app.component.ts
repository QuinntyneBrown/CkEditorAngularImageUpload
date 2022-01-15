import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { EMPTY, map, startWith, Subject, switchMap, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  private readonly _changeSubject: Subject<any> = new Subject();

  private readonly _change$ = this._changeSubject.asObservable();

  @ViewChild("input", { static: false }) public fileInput: ElementRef<HTMLInputElement>;
  
  readonly vm$ = this._change$
  .pipe(
    switchMap($event => this._handleChange($event).pipe(
      tap(response => this.formControl.setValue(`<img src="${response.url}">`)),
    )),
    startWith(null),
    map(response => {
      return {
        response
      }
    })
  )

  constructor(
    private readonly _client: HttpClient
  ) { }

  readonly formControl = new FormControl(null,[Validators.required]);

  private _handleChange(files) {
    if (files.length > 0) {
      const file = files[0];      
      const formData = new FormData();       
      formData.append("file", file, file.name);    
      return this._client.post<{ url: string }>('https://localhost:5001/api/imageUpload', formData)
    }

    return EMPTY;
  }

  onChange($event) {  
    this._changeSubject.next($event);
  }

  public handleChooseAFileClick() { this.fileInput.nativeElement.click(); }
  
}
