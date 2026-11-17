# Regras de negócio — Lap Time

## Objetivo

O **Lap Time** recebe os tempos de volta de uma prova e apresenta estatísticas da corrida. Este documento registra as regras de validação e os resultados esperados, independentemente da tecnologia usada para implementá-los.

## Registro das voltas

1. Cada tempo informado deve seguir exatamente o formato `MM:SS.mmm`: dois dígitos para minutos, dois para segundos e três para milissegundos.
2. Minutos devem estar entre `00` e `59`, inclusive.
3. Segundos devem estar entre `00` e `59`, inclusive.
4. Milissegundos devem estar entre `000` e `999`, inclusive.
5. Não são aceitos valores negativos, sinais, campos ausentes, separadores diferentes, espaços ou dígitos a mais ou a menos.
6. O tempo total da volta não pode ser zero. Assim, `00:00.000` é inválido; qualquer tempo positivo no formato válido pode ser aceito.
7. Uma prova deve conter no mínimo **3 voltas válidas** para que suas estatísticas sejam calculadas e apresentadas, e pode conter no máximo **100 voltas**.
8. Durante a entrada progressiva, podem existir de 1 a 2 voltas válidas, mas os resultados estatísticos só ficam disponíveis quando houver pelo menos 3. Uma tentativa de adicionar uma volta inválida ou exceder o limite não deve alterar as voltas já registradas.
9. A ordem de registro das voltas deve ser preservada na exibição. A ordenação necessária aos cálculos não deve reordenar a lista original.

## Cálculos

Todos os tempos devem ser convertidos para milissegundos inteiros antes dos cálculos, sem usar aproximações de ponto flutuante para armazenar as voltas.

- **Média:** soma dos tempos dividida pela quantidade de voltas. Se o resultado tiver fração de milissegundo, arredondar para o milissegundo mais próximo; valores exatamente no meio são arredondados para cima.
- **Mediana:** ordenar uma cópia dos tempos. Com quantidade ímpar, usar o valor central; com quantidade par, calcular a média dos dois valores centrais e arredondar com a mesma regra da média.
- **Moda:** apresentar o(s) tempo(s) que ocorrer(em) com maior frequência. Se todos os tempos tiverem frequências iguais, apresentar a mensagem **“Não houve moda.”** Se mais de um tempo compartilhar a maior frequência, apresentar todos eles.
- **Tempo total:** somar os tempos de todas as voltas válidas da prova.
- **Volta mais rápida:** identificar o menor tempo registrado. Se houver empate, todas as voltas empatadas são consideradas as mais rápidas.

A média e a mediana devem ser apresentadas no formato `MM:SS.mmm`. O total deve ser apresentado como duração; quando ultrapassar `59:59.999`, usar `HH:MM:SS.mmm` para não exceder o limite de minutos do formato de entrada.

## Apresentação dos resultados

1. Apresentar média, mediana, moda e tempo total quando a prova tiver pelo menos 3 voltas válidas.
2. Destacar em roxo a(s) volta(s) mais rápida(s) na lista de voltas. Em caso de empate, destacar todas.
3. O destaque visual deve vir acompanhado de uma indicação textual, como **“Volta mais rápida”**, para que a informação não dependa apenas da cor.
4. Quando ainda houver menos de 3 voltas válidas, informar que faltam voltas para calcular os resultados e indicar quantas são necessárias.
5. Mensagens de validação devem identificar o motivo da rejeição sem apagar ou modificar os dados válidos já registrados.
