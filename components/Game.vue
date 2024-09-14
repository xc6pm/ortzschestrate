<script setup lang="ts">
import { Game } from "~/models/Game"
import CapturedPieces from "./CapturedPieces.vue"
import "./style.css"
const { game } = defineProps({
  game: { type: Game, required: true },
})
</script>

<template>
  <section style="background-color: #262522; min-height: 100vh">
    <div class="container mx-auto flex flex-col lg:flex-row items-start p-8">
      <ChessBoard
        :game="game"
        style="display: inline-block"
        class="w-full lg:w-8/12 lg:mt-1"
      />
      <div
        class="flex flex-col items-start justify-center p-4 mt-4 lg:mt-0 w-full lg:w-4/12 rounded-sm"
        style="background-color: #3c3b39"
      >
        <h1 class="font-bold pb-2 text-[#C3C2C1] text-sm md:text-lg lg:text-xl">
          Game movements and scores:
        </h1>
        <CapturedPieces
          class="block my-1"
          :game="game"
          show-captured-by="white"
        />
        <CapturedPieces
          class="block my-1"
          :game="game"
          show-captured-by="black"
        />
        <MoveHistory
          :move-history="game.moveHistory"
          style="display: inline-block"
        />
      </div>
    </div>
  </section>
</template>
