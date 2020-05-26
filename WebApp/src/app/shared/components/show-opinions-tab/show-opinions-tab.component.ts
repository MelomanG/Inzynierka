import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { RateModel } from '../../models/rate';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-show-opinions-tab',
  templateUrl: './show-opinions-tab.component.html',
  styleUrls: ['./show-opinions-tab.component.scss']
})
export class ShowOpinionsTabComponent implements OnInit {
  displayedColumns: string[] = ['userName', 'userRate', 'comment'];
  tableRates:  MatTableDataSource<RateModel>; 

  @Input() rates: RateModel[];
  
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  fakeRate: number;
  
  constructor() { }

  ngOnInit(): void {
    this.fakeRate = 5;
    this.tableRates = new MatTableDataSource<RateModel>(this.rates);
    this.tableRates.paginator = this.paginator;
  }

}
