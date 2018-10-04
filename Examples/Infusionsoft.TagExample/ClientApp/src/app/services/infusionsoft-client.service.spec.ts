import { TestBed } from '@angular/core/testing';

import { InfusionsoftClientService } from './infusionsoft-client.service';

describe('InfusionsoftClientService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: InfusionsoftClientService = TestBed.get(InfusionsoftClientService);
    expect(service).toBeTruthy();
  });
});
