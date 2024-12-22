import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { Product } from '../models/product.model';  // Certifique-se de que o caminho está correto

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = 'https://localhost:7221/api/Product';  // URL da API

  constructor(private http: HttpClient) { }

  // Método para obter os produtos
  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl).pipe(
      catchError((error) => {
        console.error('Erro ao obter produtos', error);
        throw error;  // ou você pode retornar um valor padrão ou mensagem de erro
      })
    );
  }
}
