<script setup lang="ts">
import type { BoardUpdate } from "~/models/BoardUpdate"

const { moveHistory } = defineProps({
  moveHistory: { type: Array<BoardUpdate>, required: true },
})

const movePairs = computed(() =>
  moveHistory
    .slice(1)
    .reduce((pairs: Array<Array<BoardUpdate>>, move: BoardUpdate) => {
      if (!pairs.length || pairs[pairs.length - 1].length === 2) {
        pairs.push([move])
      } else {
        pairs[pairs.length - 1].push(move)
      }
      return pairs
    }, [])
)
</script>

<template>
  <ol>
    <li class="font-bold" style="color: #c3c2c1" v-for="pair in movePairs">
      {{ pair[0].toString() }}
      {{ pair.length === 2 ? " - " + pair[1].toString() : "" }}
    </li>
  </ol>
</template>

<style lang="css" scoped>
ol {
  width: 25ch;
}
</style>
