module ZenMassageApp {
    'use strict';

    export interface ISessionItem {
        name: string;
        duration: number;    
    }

    class SessionItem implements ISessionItem {
        get name(): string {
            return this.$name;
        }

        get duration(): number {
            return this.$duration;
        }

        constructor(
            private $name: string,
            private $duration: number) {
        }
    }
}