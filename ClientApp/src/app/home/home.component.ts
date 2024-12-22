import { Component, OnInit } from '@angular/core';
import { ProductService } from '../service/product.service';
import { Product } from '../models/product.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  products: Product[] = []; // Array de produtos
  loading = false; // Indicador de carregamento
  errorMessage: string | null = null; // Mensagem de erro (se houver)

  constructor(private productService: ProductService) { }

  fetchProducts(): void {
    this.loading = true;
    this.errorMessage = null;

    this.productService.getProducts().subscribe({
      next: (data: any) => {
        console.log('Dados recebidos da API:', data);

        // Verifica se a resposta é um array ou um único produto
        if (Array.isArray(data)) {
          this.products = data; // Se for um array, atribui diretamente
        } else if (data && data.productID) {
          this.products = [data]; // Se for um único produto, coloca dentro de um array
        } else {
          console.error('Estrutura inesperada dos dados:', data);
          this.products = []; // Garante que é um array vazio em caso de erro
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Erro ao buscar produtos:', error);
        this.errorMessage = 'Erro ao carregar produtos.';
        this.loading = false;
      },
    });
  }

  // Chamado quando o componente é inicializado
  ngOnInit(): void {
    console.log('Componente Home carregado');
    this.fetchProducts(); // Inicia a busca de produtos
  }
}
