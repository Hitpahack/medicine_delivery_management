import { IdentityHelper } from "./identity.helper";

class IdleTimer {

  private sessionTimeoutMinute: number;
  private onTimeout: (d?: any) => {};

  private interval: any;
  private eventHandler: any;
  private timeoutTracker: any;

  constructor({ timeout_minute, onTimeout }) {
      this.sessionTimeoutMinute = timeout_minute;
      this.onTimeout = onTimeout;

      this.eventHandler = this.updateExpiredTime.bind(this);
      this.tracker();
      this.startInterval();
  }

  startInterval() {
      this.updateExpiredTime();
      this.interval = setInterval(() => {
          const expTime = IdentityHelper.getSecureData("_expiredTime");
          const expiredTime = parseInt(expTime, 10);
          if (expiredTime < Date.now()) {
              if (this.onTimeout) {
                  this.onTimeout();
                  this.cleanUp();
              }
          }
      }, 1000);
  }

  updateExpiredTime() {
      if (this.timeoutTracker) {
          clearTimeout(this.timeoutTracker);
      }
      this.timeoutTracker = setTimeout(() => {
          var time = new Date().addMinute(this.sessionTimeoutMinute).getTime();
          IdentityHelper.setSecureData("_expiredTime",time.toString())
      }, 300);
  }

  tracker() {
      window.addEventListener("mousemove", this.eventHandler);
      window.addEventListener("scroll", this.eventHandler);
      window.addEventListener("keydown", this.eventHandler);
  }

  cleanUp() {
      clearInterval(this.interval);
      window.removeEventListener("mousemove", this.eventHandler);
      window.removeEventListener("scroll", this.eventHandler);
      window.removeEventListener("keydown", this.eventHandler);
  }
}
export default IdleTimer;
