import { Component, OnInit } from '@angular/core';
import { InfusionsoftClientService } from '../services/infusionsoft-client.service';

@Component({
  selector: 'user-input',
  templateUrl: './user-input.component.html',
  styleUrls: ['./user-input.component.scss']
})
export class UserInputComponent implements OnInit {

  email: string;
  tagId: number;

  constructor(private infService: InfusionsoftClientService) { }

  ngOnInit() {
  }

  async send()
  {
    var error = await this.infService.sendUser(this.email, this.tagId);
    if(!error)
    {
      alert(`Tag '${this.tagId}' was applied successfully to contact ${this.email}'`);
    }
    else
    {
      alert(error);
    }
  }

  tags: any[];

  async loadTags()
  {
    try
    {
      this.tags = [];
      this.tags = await this.infService.getAllTags();
    } catch (err) {
      alert(`Error getting tags: ${err}`);
    }
  }
}
