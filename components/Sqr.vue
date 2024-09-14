<script setup lang="ts">
import { Sqr as SqrType } from "~/models/Square"

const { sqr, moveTarget } = defineProps({
  sqr: { type: SqrType, required: true },
  moveTarget: { type: Boolean, required: true },
})
const emit = defineEmits<{ click: [s: SqrType] }>()
</script>

<template>
  <button v-if="sqr" :class="sqr.color" @click="() => emit('click', sqr)">
    <img
      v-if="sqr.piece"
      :src="'/pieces/' + sqr.piece.color[0] + sqr.piece.type + '.png'"
      :alt="sqr.piece.color + ' ' + sqr.piece.type"
    />
    <div class="moveTg" v-if="moveTarget"></div>
  </button>
</template>

<style scoped>
button {
  width: 100px;
  height: 100px;
  border-style: none;
  border-width: 0;
  margin-bottom: -5px;
  margin-top: -5px;
  padding: 0;
  position: relative;
}

.white {
  background-color: gainsboro;
}

.black {
  background-color: indigo;
}

img {
  width: 80%;
  height: 80%;
  position: absolute;
  margin: auto;
  left: 10%;
  right: 50%;
  top: 50%;
  bottom: 50%;
  text-align: center;
}

.moveTg {
  border-radius: 50%;
  background-color: black;
  opacity: 0.5;
  width: 35%;
  height: 35%;
  margin-left: auto;
  margin-right: auto;
}
@media (max-width: 326px) {
  button {
    width: 32px;
    height: 32px;
  }
}
@media (min-width: 327px) and (max-width: 364px) {
  button {
    width: calc(100vw / 10);
    height: calc(100vw / 10);
  }
}
@media (min-width: 365px) and (max-width: 375px) {
  button {
    width: calc(100vw / 9.7);
    height: calc(100vw / 10);
  }
}
@media (min-width: 376px) and (max-width: 425px) {
  button {
    width: calc(100vw / 9.7);
    height: calc(100vw / 9.7);
  }
}
@media (min-width: 426px) and (max-width: 450px) {
  button {
    width: calc(100vw / 9.6);
    height: calc(100vw / 9.6);
  }
}
@media (min-width: 451px) and (max-width: 500px) {
  button {
    width: calc(100vw / 9.4);
    height: calc(100vw / 9.4);
  }
}
@media (min-width: 501px) and (max-width: 550px) {
  button {
    width: calc(100vw / 9.2);
    height: calc(100vw / 9.2);
  }
}
@media (min-width: 551px) and (max-width: 600px) {
  button {
    width: calc(100vw / 9.1);
    height: calc(100vw / 9.1);
  }
}
@media (min-width: 601px) and (max-width: 647px) {
  button {
    width: calc(100vw / 9);
    height: calc(100vw / 9);
  }
}
@media (min-width: 648px) and (max-width: 662px) {
  button {
    width: calc(100vw / 9.2);
    height: calc(100vw / 9.2);
  }
}
@media (min-width: 663px) and (max-width: 676px) {
  button {
    width: calc(100vw / 9.4);
    height: calc(100vw / 9.4);
  }
}
@media (min-width: 677px) and (max-width: 691px) {
  button {
    width: calc(100vw / 9.6);
    height: calc(100vw / 9.5);
  }
}
@media (min-width: 692px) and (max-width: 705px) {
  button {
    width: calc(100vw / 9.8);
    height: calc(100vw / 9.7);
  }
}
@media (min-width: 706px) and (max-width: 767px) {
  button {
    width: 72px;
    height: calc(100vw / 9.7);
  }
}
@media (min-width: 768px) and (max-width: 1023px) {
  button {
    width: 88px;
    height: calc(100vw / 9.7);
  }
}
@media (min-width: 1024px) and (max-width: 1279px) {
  button {
    width: 76px;
    height: 76px;
  }
}
@media (min-width: 1280px) and (max-width: 1439px) {
  button {
    width: 96px;
    height: 96px;
  }
}
</style>
