import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InfusionsoftClientService 
{
  constructor(private httpClient: HttpClient) { }

  async sendUser(email: string, tagId: number) : Promise<string>
  {
    return new Promise<string>((resolve, reject) => {
      this.httpClient.post<string>(`/api/infusionsoft/senduser`, { email: email, tagId: tagId}).subscribe(async response => {
        resolve(response);
      }, error => {
        resolve(error.message);
      });
    });
  }
  async getAllTags(): Promise<any[]> {
    return new Promise<any[]>((resolve, reject) => {
      this.httpClient.get <any[]>(`/api/infusionsoft/getalltags`).subscribe(response => {
        resolve(response);
      }, error => {
        reject(error.message);
      });
    });
  }

  async getClientId() : Promise<string>
  {
    return new Promise<string>((resolve, reject) => {
      this.httpClient.get(`/api/infusionsoft/getclientid`, {responseType: 'text'}).subscribe(response => {
        resolve(response);
      }, error => {
        reject(error.message);
      });
    });
  }

  async authorize(authCode: string, redirectUrl: string = undefined) : Promise<string>
  {
    return new Promise<string>((resolve, reject) => {
      var obj;
      if(redirectUrl)
      {
        obj = {authCode: authCode, redirectUrl: redirectUrl};
      }
      else
      {
        obj = {authCode: authCode};
      }
      this.httpClient.post(`/api/infusionsoft/authorize`, obj, {responseType: 'text'}).subscribe(async response => {
        resolve(response);
      }, error => {
        reject(error.message);
      });
    });
  }

  async refreshToken() : Promise<boolean>
  {
    return new Promise<boolean>((resolve, reject) => {

      this.httpClient.get(`/api/infusionsoft/refreshToken`).subscribe(async response => {
        resolve(true);
      }, error => {
        resolve(false);
      });
    });
  }

  async test() : Promise<string>
  {
    return new Promise<string>((resolve, reject) => {

      this.httpClient.get(`/api/infusionsoft/test`, {responseType: 'text'}).subscribe(async response => {
        resolve(response);
      }, error => {
        resolve(error.message);
      });
    });
  }

  async reset() : Promise<void>
  {
    return new Promise<void>((resolve, reject) => {

      this.httpClient.post<void>(`/api/infusionsoft/reset`, {}).subscribe(async response => {
        resolve();
      }, error => {
        resolve(error.message);
      });
    });
  }

  // getOAuthUrl(): Promise<string> {
  //   return new Promise<string>(async (resolve, reject) => {
  //     var params: HttpParams = new HttpParams().set('callbackUrl', this.getCallbackUri());
  //     this.httpClient.get<string>(`/api/infusionsoft/getoauthurl`, {params: params}).subscribe(resp => {
  //       resolve(resp);
  //     }, error => {
  //       reject(error);
  //     })
  //   });
  // }

  // private getCallbackUri(): string {
  //   return `${window.document.baseURI}/infusionsoftloggedin`;
  // }
}
