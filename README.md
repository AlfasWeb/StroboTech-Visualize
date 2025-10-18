# StroboTech Visualize

# 🧭 StroboTech Visualize

**StroboTech Visualize** é uma aplicação Windows Forms desenvolvida em **C# (.NET Framework)** para leitura, análise e visualização de dados de aceleração em formato CSV.  
O software exibe gráficos interativos (via **OxyPlot**) e cálculos estatísticos como **pico, RMS, desvio padrão, velocidade RMS e frequência dominante**.

---

## 📊 Funcionalidades

- 📂 **Importar dados CSV** de aceleração (Eixos X, Y, Z).  
- 🧹 **Limpar dados** do gráfico com um clique.  
- 📈 **Visualização interativa** dos sinais nos três eixos (X, Y, Z).  
- 🔍 **Zoom e pan** com o mouse diretamente no gráfico.  
- 📋 **Painel lateral** com:
  - Legenda de cores para cada eixo  
  - Métricas calculadas automaticamente:
    - Pico de Aceleração  
    - RMS de Aceleração  
    - Desvio Padrão  
    - Velocidade RMS  
    - Frequência Dominante  

---

## 🖥️ Layout da Interface

A interface é dividida em **duas seções principais**:

| Área | Descrição |
|------|------------|
| 🎛️ Painel superior | Botões para adicionar CSV e limpar dados |
| 📈 Painel principal (OxyPlot) | Exibição do gráfico de aceleração (X, Y, Z) com zoom e pan |
| ℹ️ Painel lateral direito | Informações e métricas de análise, com legenda colorida |

### 🧩 Cores dos eixos
| Eixo | Cor |
|------|------|
| X | 🔴 Vermelho |
| Y | 🟢 Verde |
| Z | 🔵 Azul |

---

## ⚙️ Tecnologias Utilizadas

| Tecnologia | Descrição |
|-------------|------------|
| 🧱 **.NET Framework (WinForms)** | Base da aplicação |
| 📊 **OxyPlot.WindowsForms** | Biblioteca de gráficos científicos e interativos |
| 🧮 **System.Numerics / Math.NET** *(opcional)* | Cálculos estatísticos e FFT |
| 💾 **CSV Helper** *(opcional)* | Leitura e parsing de arquivos CSV |

---

## 🚀 Como Executar

1. Clone este repositório:
   ```bash
   git clone https://github.com/seuusuario/StroboTech_Visualize.git
Abra o projeto no Visual Studio.

Restaure os pacotes NuGet:
math
Install-Package OxyPlot.WindowsForms
Compile e execute (F5).

---

### 🧠 Estrutura de Código
  StroboTech_Visualize/
  
  ├── Form1.cs                # Lógica principal da aplicação
  
  ├── Form1.Designer.cs       # Layout do formulário
  
  ├── Program.cs              # Ponto de entrada
  
  ├── Resources/              # Ícones, imagens e arquivos auxiliares
  
  └── Data/                   # CSVs de exemplo (opcional)
  

---

### Principais componentes no Form1.Designer.cs

  btnAdicionar — botão “Adicionar CSV”
  
  btnLimpar — botão “Limpar Dados”
  
  plotGrafico — componente OxyPlot para exibir os sinais
  
  panelInfo — painel lateral com as métricas
  
  lblPeakAcc, lblRmsAcc, lblStdDev, lblVRms, lblFreqDom — rótulos informativos

---

###🔧 Extensões Possíveis

  🪄 Alternar entre modo curvas e modo barras via menu dropdown.
  
  📉 Adicionar filtros digitais (passa-alta, passa-baixa).
  
  📡 Importar dados de sensores em tempo real.
  
  📁 Exportar resultados (CSV, PDF, imagem do gráfico).

---

### 🧑‍💻 Autor

  Desenvolvido por: Rolly Santos e Grupo Alfas
  
  Versão: 1.0
  
  Contato: contato@alfasweb.com.br

---

### 📜 Licença

  Este projeto é distribuído sob a licença MIT.
  
  Sinta-se livre para usar, modificar e redistribuir conforme necessário.

---

💡 Nota: Este projeto foi construído para experimentos de análise de vibração e aceleração em três eixos, com foco em clareza visual e facilidade de uso.
