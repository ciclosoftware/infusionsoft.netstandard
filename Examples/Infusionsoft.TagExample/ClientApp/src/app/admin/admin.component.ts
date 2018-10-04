import { Component, OnInit } from '@angular/core';
import { InfusionsoftClientService } from '../services/infusionsoft-client.service';

@Component({
  selector: 'admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  clientid: string;
  authcode: string;

  constructor(private infService: InfusionsoftClientService) { }

  async ngOnInit() 
  {
    try
    {
      this.clientid = await this.infService.getClientId();
    }
    catch(err)
    {
      this.clientid = "No clientId configured in backend";
    }
  }

  async authorize()
  {
    if(!this.clientid)
    {
      alert('ClientId missing');
    }
    else if(!this.authcode)
    {
      alert('Please provide authCode');
    }
    else
    {
      try
      {
        var account = await this.infService.authorize(this.authcode);
        alert('Authorized, you can run a test...');
      }
      catch(err)
      {
        alert(`Error authorizing: ${err}`);
      }
    }
  }

  async test()
  {
    var msg = await this.infService.test();
    alert(msg);
  }

  async refresh()
  {
    var success = await this.infService.refreshToken();
    alert(`Refresh success: ${success}`);
  }

  async reset()
  {
    await this.infService.reset();
  }

}
