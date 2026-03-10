package printfoobaralternately

type FooBar struct {
	n int
    fooCh chan struct{}
    barCh chan struct{}
    startCh chan struct{}
}

func NewFooBar(n int) *FooBar {
	fb := &FooBar{n: n, fooCh: make(chan struct{}, 1), barCh: make(chan struct{}, 1)}
    
    fb.fooCh <- struct{}{}
    
    return fb
}

func (fb *FooBar) Foo(printFoo func()) {

	for i := 0; i < fb.n; i++ {

        <- fb.fooCh
		// printFoo() outputs "foo". Do not change or remove this line.
        printFoo()
        fb.barCh <- struct{}{}
	}
}

func (fb *FooBar) Bar(printBar func()) {

	for i := 0; i < fb.n; i++ {
        <- fb.barCh
		// printBar() outputs "bar". Do not change or remove this line.
        printBar()

        fb.fooCh <- struct{}{}
	}
}