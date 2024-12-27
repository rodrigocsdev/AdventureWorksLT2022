import { Component, OnInit } from '@angular/core';
import { ProductService } from '../service/product.service';
import { Product } from '../models/product.model';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  products: Product[] = []; // Array de produtos
  allProducts: Product[] = []; // Array de todos os produtos recebidos
  loading = false; // Indicador de carregamento
  errorMessage: string | null = null; // Mensagem de erro (se houver)
  currentPage = 1; // Página atual
  productsPerPage = 10; // Número de produtos por página
  totalPages: number = 1; // Total de páginas

  constructor(private productService: ProductService) { }

  // Função para calcular a página atual
  get paginatedProducts() {
    const startIndex = (this.currentPage - 1) * this.productsPerPage;
    const endIndex = startIndex + this.productsPerPage;
    return this.allProducts.slice(startIndex, endIndex);
  }

  // Função para buscar os produtos
  fetchProducts(): void {
    this.loading = true;
    this.errorMessage = null;

    this.productService.getProducts().subscribe({
      next: (data: any) => {
        console.log('Dados recebidos da API:', data);

        // Verifica se a resposta é um array ou um único produto
        if (Array.isArray(data)) {
          this.allProducts = data; // Se for um array, atribui diretamente
        } else if (data && data.productID) {
          this.allProducts = [data]; // Se for um único produto, coloca dentro de um array
        } else {
          console.error('Estrutura inesperada dos dados:', data);
          this.allProducts = []; // Garante que é um array vazio em caso de erro
        }
        this.totalPages = Math.ceil(this.allProducts.length / this.productsPerPage); // Calcula o total de páginas
        this.loading = false;
      },
      error: (error) => {
        console.error('Erro ao buscar produtos:', error);
        this.errorMessage = 'Erro ao carregar produtos.';
        this.loading = false;
      },
    });
  }

  // Função para mudar de página
  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }

  // Chamado quando o componente é inicializado
  ngOnInit(): void {
    console.log('Componente Home carregado');
    this.fetchProducts(); // Inicia a busca de produtos
  }
}
